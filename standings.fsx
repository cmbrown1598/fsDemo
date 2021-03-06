type Division = 
    | Rookies 
    | Minors
    | Majors 
    | MiddleSchool
    | HighSchool

type Game = { Id: int; HomeTeam: string; HomeScore: int; VisitingTeam: string; VisitorsScore: int }
type StandingsLine = { Team: string; Wins: int; Losses: int; Ties: int }


let divisionToCalendarUrlMap =
    Map.empty.
        Add(Rookies, "https://calendar.google.com/calendar/ical/ngssa.org_84gvlulr7vsnssbr0sqil5gpio%40group.calendar.google.com/public/full.ics").
        Add(Minors, "https://calendar.google.com/calendar/ical/ngssa.org_fffr1344s7f91bfj6bvr8qt82o%40group.calendar.google.com/public/full.ics").
        Add(Majors, "https://calendar.google.com/calendar/ical/ngssa.org_piht5rlh6vaj5ligc13tl6pn98%40group.calendar.google.com/public/full.ics").
        Add(MiddleSchool, "https://calendar.google.com/calendar/ical/ngssa.org_l8hrrbd2qhqdfitv2rvsvvqht8%40group.calendar.google.com/public/full.ics").
        Add(HighSchool, "https://calendar.google.com/calendar/ical/ngssa.org_36mf8jo4mpconrks5dj0g937sc%40group.calendar.google.com/public/full.ics")


open System
open System.Net
open System.Text.RegularExpressions

let downloadCalendar (url : string) = 
    use m = new WebClient()
    let results = m.DownloadString (url)
    results.Split (Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)

    
let parseGame str = 
    let reg = Regex(@"SUMMARY:Game #(\d{3}) \-? ?(\w+) \((\d+)\+?\) vs (\w+) \((\d+)\+?\)")
    let vk = reg.IsMatch str
    if(vk) then
        let matches = reg.Match str
        Some { 
                Id = Int32.Parse matches.Groups.[1].Value; 
                HomeScore = Int32.Parse matches.Groups.[5].Value; 
                HomeTeam = matches.Groups.[4].Value; 
                VisitingTeam = matches.Groups.[2].Value; 
                VisitorsScore = Int32.Parse matches.Groups.[3].Value }
    else
        None
    
let getStandingsLinesFromGame game = 
    let home = { Team = game.HomeTeam; Wins = 0; Losses = 0; Ties = 0 }
    let away =  { Team = game.VisitingTeam; Wins = 0; Losses = 0; Ties = 0 }
    
    match game.VisitorsScore - game.HomeScore with 
    | 0 ->  [
                { home with Ties = 1 }
                { away with Ties = 1 }
            ] 
    | n when n > 0 
        ->  [
                { home with Losses = 1 }
                { away with Wins = 1  }
            ] 
    | _ ->  [
                { home with Wins = 1 }
                { away with Losses = 1 }
            ] 

let combineStandingsLines s1 s2 = 
    if (s1.Team = s2.Team) then 
        { s1 with 
            Wins = s1.Wins + s2.Wins; 
            Losses = s1.Losses + s2.Losses; 
            Ties = s1.Ties + s2.Ties;  }
    else
        failwith "Teams not the same."


let rec addStandingsLineToList standingsLine standingsList = 
    match standingsList with
    | [] -> [standingsLine]
    | x::xs when (standingsLine.Team = x.Team) -> 
        let newLine = combineStandingsLines x standingsLine
        newLine::xs
    | x::xs ->
        let newList = addStandingsLineToList standingsLine xs
        x::newList

let rec combineLists list list2 = 
    match list2 with
    | [] -> list
    | x::xs -> combineLists (addStandingsLineToList x list) xs
    
let standingsVal s = 
    ((s.Wins * 2) + (s.Losses * -2) + s.Ties) * -1
    
let getCompleteStandings games = 
    games |>
        List.map getStandingsLinesFromGame |> 
        List.reduce combineLists |>
        List.sortBy standingsVal

let getDivisionGames division = 
    downloadCalendar divisionToCalendarUrlMap.[division]
                    |> Array.map parseGame
                    |> Array.filter (fun opt -> opt <> None)
                    |> Array.map Option.get
                    |> List.ofArray 

let showStandings division = 
    getDivisionGames division
        |> getCompleteStandings

let renderAsCsv game = 
    sprintf "%i,%s,%i,%s,%i" game.Id game.HomeTeam game.HomeScore game.VisitingTeam game.VisitorsScore

let csv division fileName = 
    let header = sprintf "Id,Home,HomeScore,Visitors,VisitorScore" 
    let games = getDivisionGames division 
                    |> List.map renderAsCsv
    let arr = header::games
                    |> Array.ofList
    IO.File.WriteAllLines(fileName, arr, Text.Encoding.UTF8)

