-- GetIncompleteTasks.sql
SELECT Id, Title, IsComplete
FROM Tasks
WHERE IsComplete = 0;