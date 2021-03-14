FROM vadimmakerov/dotnet-core:5.0 as builder

WORKDIR /app

ADD . .
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

RUN dotnet build -o ./package
    
WORKDIR /app

FROM vadimmakerov/dotnet-core:5.0 as final

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

WORKDIR /app
COPY --from=builder /app/package ./cmd 

CMD ["dotnet", "./cmd/EventsLogger.dll"]