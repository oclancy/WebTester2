open System.Management.Automation.Runspaces
open System.Management.Automation
open System.Collections.ObjectModel

// Learn more about F# at http://fsharp.org


let RunScript (scriptfile:string) (variable:string) : string= 

    let runspaceConfiguration = RunspaceConfiguration.Create();

    let runspace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
    runspace.Open();

    //let scriptInvoker = new RunspaceInvoke(runspace);

    use pipeline = runspace.CreatePipeline();

    //Here's how you add a new script with arguments
    let myCommand = new Command(scriptfile);
    //let testParam = new CommandParameter("key","value");
    //myCommand.Parameters.Add(testParam);

    pipeline.Commands.Add(myCommand);
//    let results = pipeline.Invoke();
    pipeline.Invoke().[0].ToString() 
    // Execute PowerShell script
    //runspace.SessionStateProxy.PSVariable.GetValue(variable).ToString() |> int

[<EntryPoint>]
let main argv =
    printf "%s" ( RunScript argv.[0] argv.[1] )
    0 // return an integer exit code
