import {
    Tag,
    Users,
    Settings,
    Bookmark,
    SquarePen,
    LayoutGrid,
    LucideIcon
  } from "lucide-react";
  import {useTranslations} from 'next-intl';

  type Submenu = {
    href: string;
    label: string;
    active: boolean;
  };
  
  type Menu = {
    href: string;
    label: string;
    active: boolean;
    icon: LucideIcon
    submenus: Submenu[];
  };
  
  type Group = {
    groupLabel: string;
    menus: Menu[];
  };
  
  export function getMenuList(pathname: string): Group[] {
    const t = useTranslations('rootlayout');

    return [
      {
        groupLabel: "",
        menus: [
          {
            href: "/dashboard",
            label: t('dashboard'),
            active: pathname.includes("/dashboard"),
            icon: LayoutGrid,
            submenus: []
          }
        ]
      },
      {
        groupLabel: "Contents",
        menus: [
          {
            href: "",
            label: t('brand.brands'),
            active: pathname.includes("/brands"),
            icon: SquarePen,
            submenus: [
              {
                href: "/brands",
                label: t('brand.all-brands'),
                active: pathname === "/brands"
              },
              {
                href: "/brands/new",
                label: t('brand.new-brand'),
                active: pathname === "/brands/new"
              }
            ]
          },
          {
            href: "",
            label: t('product.products'),
            active: pathname.includes("/products"),
            icon: SquarePen,
            submenus: [
              {
                href: "/products",
                label: t('product.all-products'),
                active: pathname === "/products"
              },
              {
                href: "/products/new",
                label: t('product.new-product'),
                active: pathname === "/products/new"
              }
            ]
          },
          {
            href: "/categories",
            label: "Categories",
            active: pathname.includes("/categories"),
            icon: Bookmark,
            submenus: []
          },
          {
            href: "/tags",
            label: "Tags",
            active: pathname.includes("/tags"),
            icon: Tag,
            submenus: []
          }
        ]
      },
      {
        groupLabel: "Settings",
        menus: [
          {
            href: "/users",
            label: "Users",
            active: pathname.includes("/users"),
            icon: Users,
            submenus: []
          },
          {
            href: "/account",
            label: "Account",
            active: pathname.includes("/account"),
            icon: Settings,
            submenus: []
          }
        ]
      }
    ];
  }