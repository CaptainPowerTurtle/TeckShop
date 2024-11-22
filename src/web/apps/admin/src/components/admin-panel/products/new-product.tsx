"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { Button } from "@teckshop/ui/components/ui/button";
import { Card, CardContent } from "@teckshop/ui/components/ui/card";
import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage } from "@teckshop/ui/components/ui/form";
import { Input } from "@teckshop/ui/components/ui/input";
import { Textarea } from "@teckshop/ui/components/ui/textarea";
import { cn } from "@teckshop/ui/lib/utils";
import { useAction } from "next-safe-action/hooks";
import { Form, useForm } from "react-hook-form";
import { toast } from "sonner";
import { z } from "zod";
import { addProductAction } from "~/src/app/actions/product-actions";
import { useRouter } from "~/src/navigation";
import { addProductSchema } from "~/src/schemas/product-schema";

export default function AddBrandForm() {
    const router = useRouter();
    const { executeAsync, result, isPending, hasErrored } = useAction(
      addProductAction,
      {
        onSuccess: ({ data }) => {
          toast.success("Brand created!", {
            description: "Brand have been created with the folowwing name",
            richColors: true,
            closeButton: true,
          });
        },
        onError: ({ error }) => {
          if (error.fetchError || error.serverError) {
            toast.error("Brand was not updated!", {
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
        website: "",
      },
    });
    // 2. Define a submit handler.
    async function onSubmit() {
      await executeAsync(form.getValues());
      if (hasErrored) {
        return;
      }
      router.refresh();
      form.reset();
    }
    return (
      <div>
        <Card>
          <CardContent className={cn("py-4")}>
            <Form {...form}>
              <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
                <div className={cn("grid grid-cols-2 gap-8")}>
                  <FormField
                    control={form.control}
                    name="name"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Name</FormLabel>
                        <FormControl>
                          <Input placeholder="shadcn" {...field} />
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
                    name="website"
                    render={({ field }) => (
                      <FormItem>
                        <FormLabel>Website</FormLabel>
                        <FormControl>
                          <Input placeholder="https://teck.dk" {...field} />
                        </FormControl>
                        <FormDescription>
                          The Website of the brand
                        </FormDescription>
                        <FormMessage />
                      </FormItem>
                    )}
                  />
                </div>
                <FormField
                  control={form.control}
                  name="description"
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel>Description</FormLabel>
                      <FormControl>
                        <Textarea
                          placeholder="Long description of the brand..."
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
                <Button type="submit">
                  {isPending && (
                    <Spinner
                      className="mr-2 size-4 animate-spin"
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
  