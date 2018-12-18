FROM microsoft/dotnet:latest
RUN dotnet tool install --global Paket
COPY . /app
WORKDIR /app
RUN ~/.dotnet/tools/paket install
EXPOSE 5000
ENV ASPNETCORE_URLS="http://*:5000"
CMD dotnet run