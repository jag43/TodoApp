$json = Get-Content '..\TodoApp\appsettings.Development.json' | Out-String | ConvertFrom-Json
$connectionString = $json.ConnectionStrings.Todo;

dotnet ef dbcontext scaffold $connectionString Microsoft.EntityFrameworkCore.SqlServer `
	--context-dir Data `
	--output-dir Models `
	--table TodoItem `
	--force