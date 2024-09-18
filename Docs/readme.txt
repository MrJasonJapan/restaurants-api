・Set Launch Profile (from Restaurants.API folder)
dotnet run --launch-profile "https"

・Dev Run (Method 1)
Set Working Folder: 「app/src/web_api/Restaurants.API/」
export ASPNETCORE_ENVIRONMENT=Development

・Dev Run (Method 2)
Set Working Folder: 「app/src/web_api/Restaurants.API/」
set ASPNETCORE_ENVIRONMENT=Development
dotnet run

・Dev Run Access URL Sample
https://localhost:7186/swagger/index.html


・Git Logs (From Restaurants.API Folder)
git diff --cached -- . ':(exclude)Docs/*' > ../Docs/GitLogs/1_current_changes.txt
    * [git diff --cached]: Shows the differences between the staged changes and the last commit (what will be committed next).
git diff --staged ':(exclude)Docs/*' > ../Docs/GitLogs/1_current_changes.txt
git log > ../Docs/GitLogs/2_all_commit_messages.txt
    
・Commit Prompt (set 1_current_changes.txt and 2_all_logs_newest_to_oldest.txt files as context)
Based on the content inside 1_current_changes.txt, please give me a thorough commit message in English.
Keep the commit message limited to recent changes only.
For understanding the project context, see 2_all_logs_newest_to_oldest.txt for a history of previous commits.

・Useful Git Commands
git show <short_hash> | clip
    * get changes of any specific commit

