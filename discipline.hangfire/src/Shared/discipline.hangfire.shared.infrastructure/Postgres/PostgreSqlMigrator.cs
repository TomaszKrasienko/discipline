using Microsoft.Extensions.Hosting;

namespace discipline.hangfire.infrastructure.Postgres;

internal sealed class PostgreSqlMigrator : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        string sql = @"
            do $$
			begin 
				IF NOT EXISTS (SELECT 1 FROM pg_catalog.pg_namespace WHERE nspname = 'centre') 
				THEN
					CREATE SCHEMA centre;
				
					CREATE TABLE centre.""ActivityRules"" (
						  activity_rule_id VARCHAR(30) NOT NULL
						, user_id VARCHAR(30) NOT NULL
						, mode VARCHAR(30) NOT NULL
						, selected_days NUMERIC[]
						, updated_at TIMESTAMP not NULL
					);

					create schema tasks
                          
					CREATE TABLE tasks.""Planned"" (
						id varchar(30) NOT NULL,
						activity_rule_id varchar(30) NOT NULL,
						user_id varchar(30) NOT NULL,
						planned_for date NOT NULL,
						state varchar(30) NOT null default 'new',
						CONSTRAINT ""Planned_activity_rule_id_user_id_planned_for_key"" UNIQUE (activity_rule_id, user_id, planned_for),
						CONSTRAINT ""Planned_pkey"" PRIMARY KEY (id)
					);
				END IF;
			END $$;";
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}