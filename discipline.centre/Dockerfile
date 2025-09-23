FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY src/modules/users/discipline.centre.users.domain/discipline.centre.users.domain.csproj ./discipline.centre.users.domain/
COPY src/modules/users/discipline.centre.users.infrastructure/discipline.centre.users.infrastructure.csproj ./discipline.centre.users.infrastructure/
COPY src/modules/users/discipline.centre.users.application/discipline.centre.users.application.csproj ./discipline.centre.users.application/
COPY src/modules/users/discipline.centre.users.api/discipline.centre.users.api.csproj ./discipline.centre.users.api/

COPY src/modules/activityRules/discipline.centre.activityrules.domain/discipline.centre.activityrules.domain.csproj ./discipline.centre.activityrules.domain/
COPY src/modules/activityRules/discipline.centre.activityrules.infrastructure/discipline.centre.activityrules.infrastructure.csproj ./discipline.centre.activityrules.infrastructure/
COPY src/modules/activityRules/discipline.centre.activityrules.application/discipline.centre.activityrules.application.csproj ./discipline.centre.activityrules.application/
COPY src/modules/activityRules/discipline.centre.activityrules.api/discipline.centre.activityrules.api.csproj ./discipline.centre.activityrules.api/

COPY src/modules/calendar/discipline.centre.calendar.domain/discipline.centre.calendar.domain.csproj ./discipline.centre.calendar.domain/
COPY src/modules/calendar/discipline.centre.calendar.infrastructure/discipline.centre.calendar.infrastructure.csproj ./discipline.centre.calendar.infrastructure/
COPY src/modules/calendar/discipline.centre.calendar.application/discipline.centre.calendar.application.csproj ./discipline.centre.calendar.application/
COPY src/modules/calendar/discipline.centre.calendar.api/discipline.centre.calendar.api.csproj ./discipline.centre.calendar.api/

COPY src/shared/discipline.centre.shared.abstractions/discipline.centre.shared.abstractions.csproj ./discipline.centre.shared.abstractions/
COPY src/shared/discipline.centre.shared.infrastructure/discipline.centre.shared.infrastructure.csproj ./discipline.centre.shared.infrastructure/

COPY src/bootstrappers/discipline.centre.bootstrappers.api/discipline.centre.bootstrappers.api.csproj ./discipline.centre.bootstrappers.api/

COPY ./../discipline.sln ./

RUN dotnet restore ./discipline.sln

COPY . ./


RUN dotnet publish discipline.sln -c Release -o /out
