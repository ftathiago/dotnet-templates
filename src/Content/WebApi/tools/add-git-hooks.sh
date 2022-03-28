#!/bin/bash
git init
npm init --yes && npx husky-init --yes && npm install && npm install lint-staged --save-dev && cp tools/pre-commit.sample .husky/pre-commit -f && cp tools/pre-push.sample .husky/pre-push -f && cp tools/post-merge.sample .husky/post-merge -f
dotnet-format
