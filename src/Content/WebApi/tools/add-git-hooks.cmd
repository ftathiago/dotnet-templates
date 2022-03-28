git init
npm ci
npx husky-init
npm install lint-staged --save-dev
copy tools\pre-commit.sample .husky\pre-commit
copy tools\pre-push.sample .husky\pre-push
copy tools\post-merge.sample .husky\post-merge
dotnet-format
