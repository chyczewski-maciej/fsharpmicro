FROM microsoft/dotnet:2.2-sdk as build
RUN dotnet tool install --global Paket
ENV PATH="/root/.dotnet/tools:${PATH}"
COPY . /app
WORKDIR /app
RUN paket install
RUN dotnet publish -c release

FROM microsoft/dotnet:2.2-runtime as run
COPY --from=build /app/bin/release/netcoreapp2.2/publish /app
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS="http://*:5000"
ENV LANG=C 
CMD dotnet app.dll