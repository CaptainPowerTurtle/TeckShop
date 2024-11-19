"use client"
"use memo"

import * as React from "react"




//import { type getTasks } from "../_lib/queries"
//import { getPriorityIcon, getStatusIcon } from "../_lib/utils"

//import { TasksTableFloatingBar } from "./tasks-table-floating-bar"
import { DataTableAdvancedToolbar } from "@repo/ui/components/data-table/advanced/data-table-advanced-toolbar"
import { DataTableToolbar } from "@repo/ui/components/data-table/data-table-toolbar"
import { DataTable  } from "@repo/ui/components/data-table/data-table"
import { useDataTable } from "@repo/ui/hooks/use-data-table"
import {  type BrandSchema} from "~/src/schemas/brand-schema"
import { type getBrandsQuery } from "~/src/lib/data/brand-queries"
import { getColumns } from "./brands-table-columns"
import { cn } from "@repo/ui/lib/utils"
import { useBrandsTable } from "./brands-table-provider"
import { BrandsTableToolbarActions } from "./brands-table-toolbar-actions"
import { DataTableFilterField } from "@repo/ui/types/index"

interface BrandsTableProps {
  brandsPromise: ReturnType<typeof getBrandsQuery>
}

export function BrandsTable({ brandsPromise }: BrandsTableProps) {
  // Feature flags for showcasing some additional features. Feel free to remove them.
  const { featureFlags } = useBrandsTable()

  const {data, totalPages, totalItems} = React.use(brandsPromise)
  // Memoize the columns so they don't re-render on every render
  const columns = React.useMemo(() => getColumns(), [])

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
  const filterFields: DataTableFilterField<BrandSchema>[] = [
    {
        label: "Name",
        value: "name"
    },
    {
        label: "Website",
        value: "website"
    }
    // {
    //   label: "Status",
    //   value: "status",
    //   options: tasks.status.enumValues.map((status) => ({
    //     label: status[0]?.toUpperCase() + status.slice(1),
    //     value: status,
    //     icon: getStatusIcon(status),
    //     withCount: true,
    //   })),
    // },
    // {
    //   label: "Priority",
    //   value: "priority",
    //   options: tasks.priority.enumValues.map((priority) => ({
    //     label: priority[0]?.toUpperCase() + priority.slice(1),
    //     value: priority,
    //     icon: getPriorityIcon(priority),
    //     withCount: true,
    //   })),
    // },
  ]

  const { table } = useDataTable({
    data,
    columns,
    pageCount: totalPages,
    rowCount: totalItems,
    /* optional props */
    filterFields,
    enableAdvancedFilter: featureFlags.includes("advancedFilter"),
    // initialState: {
    //   sorting: [{ id: "id", desc: true }],
    //   columnPinning: { right: ["actions"] },
    // },
    // For remembering the previous row selection on page change
    getRowId: (originalRow, index) => `${originalRow.id}-${index}`,
    /* */
  })

  return (
    <DataTable
      table={table}
      // floatingBar={
      //   featureFlags.includes("floatingBar") ? (
      //     <TasksTableFloatingBar table={table} />
      //   ) : null
      // }
    >
      {featureFlags.includes("advancedFilter") ? (
        <DataTableAdvancedToolbar table={table} filterFields={filterFields}>
          <BrandsTableToolbarActions table={table} />
        </DataTableAdvancedToolbar>
      ) : (
        <DataTableToolbar  table={table} filterFields={filterFields}>
          <BrandsTableToolbarActions table={table} />
        </DataTableToolbar>
      )}
    </DataTable>
  )
}
