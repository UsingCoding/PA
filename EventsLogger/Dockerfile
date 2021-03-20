FROM vadimmakerov/dotnet-core:5.0

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

WORKDIR /app
ADD ./bin/EventsLogger ./cmd 

CMD ["dotnet", "./cmd/EventsLogger.dll"]