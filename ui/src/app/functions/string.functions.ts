/**
 * Converts PascalCase or camelCase enum keys to a human-readable string.
 * Example: "SignedOfferLetter" → "Signed Offer Letter"
 */
export function humanizeEnumKey(key: string): string {
  return key
    .replace(/([a-z])([A-Z])/g, '$1 $2')      // insert space before capital letters
    .replace(/([A-Z])([A-Z][a-z])/g, '$1 $2')  // handle ALLCAPS followed by PascalCase
    .replace(/^./, str => str.toUpperCase()); // capitalize first letter
}
