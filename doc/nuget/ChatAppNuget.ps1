# add "Grpc/GrpcServer" project
$projGrpcServer = "../../Grpc/GrpcServer/GrpcServer.csproj"
dotnet add $projGrpcServer package Azure.Data.Tables --version 12.5.0

# add "Grpc/GrpcShared" project
$projGrpcShared = "../../Grpc/GrpcShared/GrpcShared.csproj"
dotnet add $projGrpcShared package MagicOnion.MSBuild.Tasks --version 4.4.0
