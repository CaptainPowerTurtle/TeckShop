import createNextIntlPlugin from 'next-intl/plugin';
/**
 * Run `build` or `dev` with `SKIP_ENV_VALIDATION` to skip env validation. This is especially useful
 * for Docker builds.
 */
const withNextIntl = createNextIntlPlugin('./src/config/i18n/i18n.ts');
await import("./src/env.js");

/** @type {import("next").NextConfig} */
const nextConfig = {
  transpilePackages: ["@repo/ui"],
  // experimental: {
  //   ppr:  true,
  //   reactCompiler: {
  //     compilationMode: 'annotation',
  //   },
  // },
};


export default withNextIntl(nextConfig);
