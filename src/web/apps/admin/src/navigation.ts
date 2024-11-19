import {createSharedPathnamesNavigation} from 'next-intl/navigation';
import {  localeCodes as locales } from './config/i18n/config';


export const {Link, redirect, usePathname, useRouter} =
  createSharedPathnamesNavigation({locales});