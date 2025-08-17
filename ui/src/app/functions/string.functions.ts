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

export function copyToClipboard(text: string): void {
  if (navigator.clipboard) {
    navigator.clipboard.writeText(text).then(
      () => console.info('Copied to clipboard', text),
      (err) => console.error('Could not copy text: ', err)
    );
  } else {
    // Fallback for older browsers
    const textarea = document.createElement('textarea');

    textarea.value = text;
    textarea.style.position = 'fixed';
    textarea.style.opacity = '0';

    document.body.appendChild(textarea);

    textarea.focus();
    textarea.select();

    try {
      document.execCommand('copy');
      console.log('[LEGACY] Copied to clipboard', text);
    } catch (err) {
      console.error('[LEGACY]: Could not copy text', err);
    }

    document.body.removeChild(textarea);
  }
}
