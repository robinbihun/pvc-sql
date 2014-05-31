pvc.Task("nuget-push", () => {
    pvc.Source("src/Pvc.Sql.csproj")
       .Pipe(new PvcNuGetPack(
            createSymbolsPackage: true
       ))
       .Pipe(new PvcNuGetPush());
});
