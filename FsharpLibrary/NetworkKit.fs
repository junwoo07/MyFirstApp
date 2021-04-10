// Learn more about F# at http://fsharp.org

namespace NetworkKit
    
open System

module Matrix =
    type Matrix2D (arg:float[][]) =
        member private this.array = arg
        override this.ToString() =
            let mutable output = "[|\n"
            for i in this.array do
                output <- output+"  [| "
                for j in i do
                    output <- output+j.ToString()+" "
                output <- output+"|]\n"
            output+"|]"
        member this.Length = this.array.Length
        member this.ToArray() = this.array
        static member (~-) (v : Matrix2D) =
            Matrix2D ( Array.map (fun m -> Array.map (fun n -> -n) m) v.array )
        static member (+) (v : Matrix2D,a : float) =
            Matrix2D ( Array.map (fun m -> Array.map (fun n -> n+a) m) v.array )
        static member (+) (a : float,v : Matrix2D) =
            v+a
        static member (+) (v : Matrix2D,a : Matrix2D) =
            Matrix2D ( Array.map2 (fun k l -> (Array.map2 (fun m n -> m+n) k l)) v.array a.array )
        static member (-) (v,a) =
            v + -a
        static member (*) (v : Matrix2D,a : float) =
            Matrix2D ( Array.map (fun m -> Array.map (fun n -> n*a) m) v.array )
        static member (*) (a : float,v : Matrix2D) =
            v*a
        static member (*) (v : Matrix2D,a : Matrix2D) =
            Matrix2D ( Array.map2 (fun k l -> (Array.map2 (fun m n -> m*n) k l)) v.array a.array )
        static member (/) (v : Matrix2D,a : float) =
            Matrix2D ( Array.map (fun m -> Array.map (fun n -> n/a) m) v.array )
        member this.T =
            Matrix2D ( Array.mapi (fun i m -> [|for j in 0..this.array.Length-1 -> this.array.[j].[i]|]) (Array.create (this.array.[0].Length) (this.array.Length)) )
        member this.Item(i : int) = this.array.[i]
        member this.map (func) = Array.map (fun m -> Array.map func m) this.array |> Matrix2D

    let dot (x:Matrix2D) (y:Matrix2D) =
        if x.ToArray().[0].Length = y.ToArray().Length 
        then Matrix2D (Array.map (fun k -> (Array.map (fun l -> (Array.sum (Array.map2 (fun m n -> m*n) k l))) (y.T.ToArray()))) (x.ToArray()))
        else raise (ArgumentException("Fail matrix multiplication"))
    let create (shape:int[]) (value:float) =
        if shape.Length = 2 then Matrix2D (Array.map (fun m -> (Array.create shape.[0] value)) (Array.zeroCreate shape.[1])) else raise (ArgumentException("the shape of \'shape\' is not (2,)","shape"))
    let randomCreate (shape:int[]) (scale:float) =
        if shape.Length = 2 then Matrix2D (Array.map (fun m -> (Array.create shape.[0] (Random().NextDouble()*scale))) (Array.zeroCreate shape.[1])) else raise (ArgumentException("the shape of \'shape\' is not (2,)","shape"))

module NetWork =
    let sigmoid x = (float 1)/(float 1 - Math.Exp(-x))
    let sigmoid' x = (sigmoid x)*(float 1 - sigmoid x)

    type SimpleNetWork (weights:Matrix.Matrix2D[],deflections:Matrix.Matrix2D[]) =
        member this.weights = weights
        member this.deflections = deflections
        member private this.JudgeFunc (input:Matrix.Matrix2D) (stack:int) =
            if stack < this.weights.Length
            then this.JudgeFunc ((Matrix.dot input this.weights.[stack].T + this.deflections.[stack]).map sigmoid) (stack+1)
            else input.map sigmoid
        member this.GetJudgeValues (input:Matrix.Matrix2D) = this.JudgeFunc (Matrix.dot input this.weights.[0].T) 1
        member this.Optimize () = 1;
    
    let w = [|Matrix.Matrix2D [|[|1.0;1.0;1.0|];[|1.0;1.0;1.0|]|]|]
    let b = [|Matrix.Matrix2D [|[|1.0;1.0|]|]|]
    let a = SimpleNetWork (w,b)
    printf $"{a.GetJudgeValues (Matrix.Matrix2D [|[|1.0;1.0;1.0|]|])}"

