"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@teckshop/ui/components/ui/button";
import { Card, CardContent } from "@teckshop/ui/components/ui/card";
import { Command, CommandEmpty, CommandGroup, CommandInput, CommandItem, CommandList } from "@teckshop/ui/components/ui/command";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@teckshop/ui/components/ui/form";
import { Input } from "@teckshop/ui/components/ui/input";
import { Popover, PopoverContent, PopoverTrigger } from "@teckshop/ui/components/ui/popover";
import { Spinner } from "@teckshop/ui/components/ui/spinner";
import { Switch } from "@teckshop/ui/components/ui/switch";
import { Textarea } from "@teckshop/ui/components/ui/textarea";
import { cn } from "@teckshop/ui/lib/utils";
import { Check, ChevronsUpDown } from "lucide-react";
import { useAction } from "next-safe-action/hooks";
import { useForm } from "react-hook-form";
import { toast } from "sonner";
import { z } from "zod";
import { addProductAction } from "~/src/app/actions/product-actions";
import { useRouter } from "~/src/navigation";
import { BrandSchema, BrandsList } from "~/src/schemas/brand-schema";
import { addProductSchema } from "~/src/schemas/product-schema";

interface AddProductProps{
  brands: BrandsList;
}


export default function AddProductForm(props: AddProductProps) {
  const router = useRouter();
  const { executeAsync, result, isPending, hasSucceeded } = useAction(
    addProductAction,
    {
      onSuccess: ({ data }) => {
        if (data?.success) {
          toast.success("Product created!", {
            description: `Product have been created with the folowwing name ${data?.success.name}`,
            richColors: true,
            closeButton: true,
          });
          router.refresh();
          form.reset();
        } else {
          data?.failure.errors.forEach((error) => {
            toast.error(data?.failure.title, {
              description: error.reason,
              richColors: true,
              closeButton: true,
              duration: Infinity,
            });
          });
        }
      },
      onError: ({ error }) => {
        if (error.fetchError) {
          toast.error("Product was not updated!", {
            description:
              error.serverError != null ? error.serverError : error.fetchError,
            richColors: true,
            closeButton: true,
            duration: Infinity,
          });
        } else if (error.serverError) {
          toast.error("Product was not updated!", {
            description:
              error.serverError != null ? error.serverError : error.fetchError,
            richColors: true,
            closeButton: true,
            duration: Infinity,
          });
        } else if (error.validationErrors) {
          toast.error("Validation error!", {
            description: "One or more validation failed!",
            richColors: true,
          });
        }
      },
    },
  );

  const form = useForm<z.infer<typeof addProductSchema>>({
    resolver: zodResolver(addProductSchema),
    defaultValues: {
      name: "",
      description: "",
      productSKU: "",
      gtin: "",
      isActive: false,
    },
  });
  // 2. Define a submit handler.
  async function onSubmit() {
    await executeAsync(form.getValues());
  }
  return (
    <div>
      <Card>
        <CardContent className={cn("py-4")}>
          <Form {...form}>
            <form
              onSubmit={(e) => {
                e.preventDefault();
                form.handleSubmit(onSubmit)();
              }}
              className="space-y-8"
            >
              <div className={cn("grid grid-cols-2 gap-4")}>
                <FormField
                  control={form.control}
                  name="name"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Name</FormLabel>
                      <FormControl>
                        <Input placeholder="Product name..." {...field} />
                      </FormControl>
                      <FormDescription>
                        This is your public display name.
                      </FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="productSKU"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>SKU</FormLabel>
                      <FormControl>
                        <Input placeholder="Sku...." {...field} />
                      </FormControl>
                      <FormDescription>The SKU of the product</FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="gtin"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Website</FormLabel>
                      <FormControl>
                        <Input placeholder="GTIN...." {...field} />
                      </FormControl>
                      <FormDescription>The gtin of the product</FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="isActive"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Website</FormLabel>
                      <FormControl>
                        <Switch
                          checked={field.value}
                          onCheckedChange={field.onChange}
                        />
                      </FormControl>
                      <FormDescription>The gtin of the product</FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="description"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Description</FormLabel>
                      <FormControl>
                        <Textarea
                          placeholder="Long description of the product..."
                          {...field}
                        />
                      </FormControl>
                      <FormDescription>
                        This is your public display name.
                      </FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />
                <FormField
                  control={form.control}
                  name="brandId"
                  render={({ field }) => (
                    <FormItem className="flex flex-col">
                      <FormLabel>Brands</FormLabel>
                      <Popover>
                        <PopoverTrigger asChild>
                          <FormControl>
                            <Button
                              variant="outline"
                              role="combobox"
                              className={cn(
                                "w-[200px] justify-between",
                                !field.value && "text-muted-foreground",
                              )}
                            >
                              {field.value
                                ? props.brands.find(
                                    (brand: BrandSchema) =>
                                      brand.id === field.value,
                                  )?.name
                                : "Select language"}
                              <ChevronsUpDown className="ml-2 h-4 w-4 shrink-0 opacity-50" />
                            </Button>
                          </FormControl>
                        </PopoverTrigger>
                        <PopoverContent className="w-[200px] p-0">
                          <Command>
                            <CommandInput placeholder="Search language..." />
                            <CommandList>
                              <CommandEmpty>No brands found.</CommandEmpty>
                              <CommandGroup>
                                {props.brands.map((brand) => (
                                  <CommandItem
                                    value={brand.name}
                                    key={brand.id}
                                    onSelect={() => {
                                      form.setValue("brandId", brand.id);
                                    }}
                                  >
                                    {brand.name}
                                    <Check
                                      className={cn(
                                        "ml-auto",
                                        brand.id === field.value
                                          ? "opacity-100"
                                          : "opacity-0",
                                      )}
                                    />
                                  </CommandItem>
                                ))}
                              </CommandGroup>
                            </CommandList>
                          </Command>
                        </PopoverContent>
                      </Popover>
                      <FormDescription>
                        This is the brand of the product.
                      </FormDescription>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              </div>
              <Button type="submit">
                {isPending && (
                  <Spinner
                    className="mr-2 size-4 animate-spin text-white dark:text-slate-950"
                    aria-hidden="true"
                  />
                )}
                Save
              </Button>
              {/* <Button disabled={isPending} variant='default' type="submit">{isPending ? <><Spinner className="text-zinc-50 dark:text-slate-950" size='small' /></> : 'Submit'}</Button> */}
            </form>
          </Form>
        </CardContent>
      </Card>
    </div>
  );
}
