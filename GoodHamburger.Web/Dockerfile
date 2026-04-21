FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GoodHamburger.Web.csproj", "."]
RUN dotnet restore "./GoodHamburger.Web.csproj"
COPY . .
RUN dotnet publish "GoodHamburger.Web.csproj" -c Release -o /app/publish

FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
