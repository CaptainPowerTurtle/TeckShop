import en from './src/langs/en.json';
//import fr from './langs/fr.json';
 
type Messages = typeof en;
//type Messages = typeof fr;

declare global {
  // Use type safe message keys with `next-intl`
  interface IntlMessages extends Messages {}
}