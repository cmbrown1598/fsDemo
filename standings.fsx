type Game = { Id: int; HomeTeam: string; HomeScore: int; VisitingTeam: string; VisitorsScore: int }
type StandingsLine = { Team: string; Wins: int; Losses: int; Ties: int }

let createGame id (visitor, vscore) (home, score) = 
    { 
        Id= id; 
        HomeTeam = home;
        HomeScore = score; 
        VisitingTeam = visitor;
        VisitorsScore = vscore 
    }

let getStandingsFromGame game = 
    match game.VisitorsScore - game.HomeScore with 
    | 0 ->  [
                { Team = game.HomeTeam; Wins = 0; Losses = 0; Ties = 1 }
                { Team = game.VisitingTeam; Wins = 0; Losses = 0; Ties = 1 }
            ] 
    | n when n > 0 
        ->  [
                { Team = game.HomeTeam; Wins = 0; Losses = 1; Ties = 0 }
                { Team = game.VisitingTeam; Wins = 1; Losses = 0; Ties = 0 }
            ] 
    | _ ->  [
                { Team = game.HomeTeam; Wins = 1; Losses = 0; Ties = 0 }
                { Team = game.VisitingTeam; Wins = 0; Losses = 1; Ties = 0 }
            ] 

let combineStandingsLines s1 s2 = 
    if (s1.Team = s2.Team) then 
        { s1 with 
            Wins = s1.Wins + s2.Wins; 
            Losses = s1.Losses + s2.Losses; 
            Ties = s1.Ties + s2.Ties;  }
    else
        failwith "Teams not the same."


let rec addToList standingsLine standingsList = 
    match standingsList with
    | [] -> [standingsLine]
    | x::xs -> 
        if (standingsLine.Team = x.Team) then
            let newLine = combineStandingsLines x standingsLine
            newLine::xs
        else
            let newList = addToList standingsLine xs
            x::newList

let rec sumLists list list2 = 
    match list2 with
    | [] -> list
    | [a] -> addToList a list
    | x::xs -> sumLists (addToList x list) xs
    
let standingsVal s = 
    ((float s.Wins * 1.0) + (float s.Losses * -1.0) + (float s.Ties) * 0.5) * -1.0
    
let getCompleteStandings games = 
    List.map getStandingsFromGame games |> 
        List.reduce sumLists |>
        List.sortBy standingsVal

