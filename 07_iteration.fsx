let printNumbers min max = 
    for m in min..max do
        printfn "%i" m













let printNumber floatVal = 
    printfn "Printing %f!" floatVal


let printMoreNumbers (minimum:float) (maximum:float) = 
    List.iter printNumber [minimum..0.45..maximum]
















let printEvenMoreNumbers (minimum:float) (maximum:float) =
    [minimum..0.45..maximum] |>
        List.filter (fun x -> x%1.0 = 0.00) |>
        List.iter printNumber 