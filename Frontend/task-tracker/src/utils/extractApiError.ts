import axios from "axios";

export function extractApiError(err: unknown): string {
  if (axios.isAxiosError(err)) {
    if (!err.response) {
      return "Unable to reach the server. Please try again.";
    }

    const data = err.response.data as any;

    if (data?.detail) {
      return data.detail;
    }

    if (typeof data === "string") {
      return data;
    }

    return `Request failed with status ${err.response.status}`;
  }

  return "Something went wrong.";
}
