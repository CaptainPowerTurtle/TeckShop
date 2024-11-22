"use client"
"use memo"

import * as React from "react"




//import { type getTasks } from "../_lib/queries"
//import { getPriorityIcon, getStatusIcon } from "../_lib/utils"

//import { TasksTableFloatingBar } from "./tasks-table-floating-bar"
import { DataTableAdvancedToolbar } from "@teckshop/ui/components/data-table/advanced/data-table-advanced-toolbar"
import { DataTableToolbar } from "@teckshop/ui/components/data-table/data-table-toolbar"
import { DataTable  } from "@teckshop/ui/components/data-table/data-table"
import { useDataTable } from "@teckshop/ui/hooks/use-data-table"
import {  type BrandSchema} from "~/src/schemas/brand-schema"
import { type getBrandsQuery } from "~/src/lib/data/brand-queries"
import { cn } from "@teckshop/ui/lib/utils"
import { DataTableFilterField, DataTableRowAction } from "@teckshop/ui/types/index"
import { Product } from "~/src/schemas/product-schema"
import { ProductsTableToolbarActions } from "./products-table-toolbar-actions"
import { UpdateProductSheet } from "./update-product-sheet"
import { DeleteProductsDialog } from "./delete-products-dialog"
import { getProductColumns } from "./products-table-columns"
import { getPagedProductsQuery } from "~/src/lib/data/product-queries"

// interface BrandsTableProps {
//   brandsPromise: ReturnType<typeof getBrandsQuery>
// }


interface ProductsTableProps {
  promises: Promise<
    [
      Awaited<ReturnType<typeof getPagedProductsQuery>>,
    ]
  >
}

export function ProductsTable({ promises }: ProductsTableProps) {
  // const { featureFlags } = useFeatureFlags()

  const [{ data, totalPages, totalItems }] =
    React.use(promises)

  const [rowAction, setRowAction] =
    React.useState<DataTableRowAction<Product> | null>(null)

    const columns = React.useMemo(() => getProductColumns(), [])
  // const columns = React.useMemo(
  //   () => getColumns({ setRowAction }),
  //   [setRowAction]
  // )

  /**
   * This component can render either a faceted filter or a search filter based on the `options` prop.
   *
   * @prop options - An array of objects, each representing a filter option. If provided, a faceted filter is rendered. If not, a search filter is rendered.
   *
   * Each `option` object has the following properties:
   * @prop {string} label - The label for the filter option.
   * @prop {string} value - The value for the filter option.
   * @prop {React.ReactNode} [icon] - An optional icon to display next to the label.
   * @prop {boolean} [withCount] - An optional boolean to display the count of the filter option.
   */
  const filterFields: DataTableFilterField<Product>[] = [
    {
      id: "name",
      label: "Name",
      placeholder: "Filter names...",
    },
  ]

  const { table } = useDataTable({
    data,
    columns,
    pageCount: totalPages,
    rowCount: totalItems,
    filterFields,
    enableAdvancedFilter: false,
    initialState: {
      sorting: [{ id: "name", desc: true }],
      columnPinning: { right: ["actions"] },
    },
    getRowId: (originalRow, index) => `${originalRow.id}-${index}`,
    shallow: false,
    clearOnDefault: true,
  })

  return (
    <>
      <DataTable
        table={table}
        floatingBar={ null}
      >
          <DataTableToolbar table={table} filterFields={filterFields}>
            <ProductsTableToolbarActions table={table} />
          </DataTableToolbar>
      </DataTable>
      <UpdateProductSheet
        open={rowAction?.type === "update"}
        onOpenChange={() => setRowAction(null)}
        product={rowAction?.row.original ?? null}
      />
      <DeleteProductsDialog
        open={rowAction?.type === "delete"}
        onOpenChange={() => setRowAction(null)}
        products={rowAction?.row.original ? [rowAction?.row.original] : []}
        showTrigger={false}
        onSuccess={() => rowAction?.row.toggleSelected(false)}
      />
    </>
  )
}