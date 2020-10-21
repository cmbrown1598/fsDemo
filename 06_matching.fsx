let greet name = 
    match System.DateTime.Now.Hour with 
    | hr when hr < 12 -> sprintf "Good morning %s!" name
    | hr when hr < 17 ->  sprintf "Good afternoon %s!" name
    | _ -> sprintf "Good evening, %s!" name



let listLength list = 
    match list with 
    | [] -> 0
    | [a] -> 1







let length list = 
    let rec loop acc =  // rec = recursion
        match acc with
        | head::tail -> 1 + loop tail
        | [] -> 0
    loop list






type Shape = 
    | Circle of Radius : float
    | Square of SideLength : float
    | Rectangle of LongSide : float * ShortSide : float
    | Line


let calcArea shape = 
    match shape with
    | Circle (r) -> System.Math.PI * r * r
    | Square (s) -> s * s
    | Rectangle (l, s) -> s * l
