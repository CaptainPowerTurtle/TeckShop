/** @type {import("eslint").Linter.Config} */
module.exports = {
  root: true,
  extends: ["@teckshop/eslint-config/next.js"],
  parser: "@typescript-eslint/parser",
  parserOptions: {
    project: true,
  },
  rules: {
    'react/jsx-no-literals': 'error'
  }
};
