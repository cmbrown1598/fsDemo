

let greet name = 
    let hr = System.DateTime.Now.Hour
    if hr < 12 then sprintf "Good morning %s!" name
    elif hr < 17 then sprintf "Good afternoon %s!" name
    sprintf "Good evening, %s!" name