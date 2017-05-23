let greet name = 
    match System.DateTime.Now.Hour with 
    | hr when hr < 12 -> sprintf "Good morning %s!" name
    | hr when hr < 17 ->  sprintf "Good afternoon %s!" name
    //| _ -> sprintf "Good evening, %s!" name










let length list = 
    let rec loop acc =  // rec = recursion
        match acc with
        | head::tail -> 1 + loop (tail)
        | [] -> 0
    loop list