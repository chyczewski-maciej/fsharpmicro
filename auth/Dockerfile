FROM microsoft/dotnet:latest
RUN dotnet tool install --global Paket
ENV PATH="/root/.dotnet/tools:${PATH}"
COPY . /app
WORKDIR /app
RUN paket install
EXPOSE 5000
ENV ASPNETCORE_URLS="http://*:5000"
CMD dotnet run
