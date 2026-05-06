export interface JwtPayload {
  sub: string;
  email: string;
  nome: string;
  tenant_id: string;
  permissions: string[];
  exp: number;
  iat: number;
}

export interface LoginRequest {
  email: string;
  senha: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
}

export interface AuthSession {
  accessToken: string;
  refreshToken: string;
  expiresAt: number;
  user: {
    id: string;
    email: string;
    nome: string;
    tenantId: string;
    permissions: string[];
  };
}
