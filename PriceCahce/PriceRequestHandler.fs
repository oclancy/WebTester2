module Handler

open Messages.Handlers
open Messages
open RabbitMqClient

type PriceRequestHandler( client: Client )  = 
    let m_client = client;
    
    interface IHandleMessage<PriceRequestMessage>
        with  member __.Handle(m) =
                        async{
                            m_client.Send( new PriceMessage() );
                        }|> Async.StartAsTask :> _
    
    
    

