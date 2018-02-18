// Learn more about F# at http://fsharp.org

open System
open RabbitMqClient
open Messages.Handlers
open Messages
open Handler

[<EntryPoint>]
let main argv =
    
    let client = new RabbitMqClient.Client( argv.[1], argv.[2], argv.[3] );
    
    let observable = client.MessageRecieved

    let despatcher = new Messages.MessageDespatcher();
    
    observable.Subscribe( despatcher ) |> ignore
    
    despatcher.RegisterHandler<StopMessage>(new StopHandler());
    despatcher.RegisterHandler<PriceRequestMessage>(new PriceRequestHandler(client) );

    Console.ReadKey() |> ignore;

    0 // return an integer exit code
