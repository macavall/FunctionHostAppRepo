# FunctionHostAppRepo

.NET 8 isolated Azure Functions app scaffolded to pair with **Lab 1** of the
Function Host training (see `FunctionHostLabRepo/labs/lab1-*`).

## Purpose

This app contains a single `QueueProcessor` function that listens on the queue
`my-queue`. It is designed to be deployed alongside the **broken `host.json`**
from [`lab1-files/host.json`](../FunctionHostLabRepo/labs/lab1-files/host.json),
so students can observe the host failing to start and then fix the errors.

## Project layout

```
FunctionHostAppRepo/
├── FunctionHostAppRepo.csproj    # .NET 8 isolated worker
├── Program.cs                    # Default FunctionsApplication builder
├── QueueProcessor.cs             # [QueueTrigger("my-queue")] function
├── host.json                     # Default (correct) host.json from func init
├── local.settings.json           # Local connection strings (gitignored in prod)
└── .vscode/                      # VS Code tasks/launch
```

## Run locally

```powershell
# 1. Start Azurite or set AzureWebJobsStorage to a real storage account
#    in local.settings.json
# 2. Build + run
func start
```

Send a message to the `my-queue` queue to trigger the function.

## Use with Lab 1 (broken host.json)

1. Deploy the Lab 1 ARM template from the sibling repo.
2. Publish this app to the Function App:
   ```powershell
   func azure functionapp publish <your-function-app-name>
   ```
3. **Replace the good `host.json`** in the deployed app's `site/wwwroot/` with
   the broken one from `FunctionHostLabRepo/labs/lab1-files/host.json` via
   Kudu.
4. Observe the host failing to index functions — the Portal will show errors.
5. Work through [`lab1-instructions.txt`](../FunctionHostLabRepo/labs/lab1-instructions.txt)
   to fix `host.json` until the `QueueProcessor` function loads.

## Why queue trigger?

Lab 1's broken `host.json` includes misconfigured `extensions.queues` settings
(`batchSize` and a bogus `queueName`). A queue-triggered function is the most
direct way to demonstrate:

- How `extensions.queues.batchSize` is a **host-wide default** that applies
  to every queue trigger (correct placement → `host.json`).
- Why `queueName` does **not** belong under `extensions.queues` — it's a
  per-function setting that lives on the binding (`[QueueTrigger("my-queue")]`
  in code, or `bindings[].queueName` in `function.json`).
