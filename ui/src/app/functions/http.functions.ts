export function cleanRequestParams(obj: Record<string, any>): Record<string, string> {
  const cleaned: Record<string, string> = {};
  for (const [key, value] of Object.entries(obj)) {
    if (value !== null && value !== undefined && value !== '') {
      cleaned[key] = String(value);
    }
  }
  return cleaned;
}
