FROM vadimmakerov/dotnet-core:5.0 as final

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

WORKDIR /app
ADD ./bin/Valuator ./cmd  

CMD ["dotnet", "./cmd/Valuator.dll", "--urls", "http://*:5000"]