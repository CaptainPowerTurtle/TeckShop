//export const locales = ['en', 'dk']

export const locales = [
    { code: "en", name: "English", flag: 'https://flagcdn.com/gb.svg'},
    { code: "dk", name: "Dansk", flag: "https://flagcdn.com/dk.svg" },
  ]
export const localeCodes = locales.map(x => x.code);

export const defaultLocale = locales[1]?.code