export function upperCaseFirstLetter(str: string): string {
   return [...str][0].toUpperCase() + str.slice(1);
}
export function lowerCaseFirstLetter(str: string): string {
   return [...str][0].toLowerCase() + str.slice(1);
}
