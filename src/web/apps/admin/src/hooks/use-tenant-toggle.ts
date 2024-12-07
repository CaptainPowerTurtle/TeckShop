import { create } from 'zustand';
import { persist, createJSONStorage } from 'zustand/middleware';
import { Organization } from '../schemas/keycloak-schema';

interface useTenantToggleStore {
    tenant: Organization | null;
    setTenant: (tenant: Organization) => void;
    getTenant: () => Organization | null;
  }
  
  export const useTenantToggleStore = create(
    persist<useTenantToggleStore>(
      (set, get) => ({
        tenant: null,
        setTenant: (tenant: Organization) => {
          set({ tenant: tenant });
        },
        getTenant: () => get().tenant
      }),
      {
        name: 'tenant',
        storage: createJSONStorage(() => sessionStorage)
      }
    )
  );