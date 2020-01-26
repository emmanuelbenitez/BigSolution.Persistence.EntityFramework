& dotnet test --collect:"Code Coverage" /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=../../tests/
& reportgenerator -reports:tests/**/coverage*.cobertura.xml -targetdir:../CodeCoverage -reporttypes:HtmlInline_AzurePipelines`;Cobertura
ii ../CodeCoverage\index.htm
