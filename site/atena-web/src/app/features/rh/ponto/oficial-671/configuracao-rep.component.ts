import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Oficial671Service, ConfiguracaoRepPayload, TipoRep } from './oficial-671.service';

@Component({
  standalone: true,
  selector: 'atena-config-rep',
  imports: [CommonModule, FormsModule],
  template: `
    <h2>Configuração REP (Portaria 671)</h2>
    <p>Dados da empresa para emissão de comprovantes assinados ICP-Brasil + exportação AFD/AEJ.</p>

    <form (ngSubmit)="salvar()" #f="ngForm">
      <label>Empresa ID <input [(ngModel)]="modelo.empresaId" name="empresaId" required /></label>
      <label>Tipo
        <select [(ngModel)]="modelo.tipo" name="tipo">
          <option value="RepC">REP-C (Cloud)</option>
          <option value="RepP">REP-P (Programa)</option>
        </select>
      </label>
      <label>Razão social <input [(ngModel)]="modelo.razaoSocial" name="rs" required /></label>
      <label>CNPJ/CEI <input [(ngModel)]="modelo.cnpjCei" name="cnpj" required maxlength="14" /></label>
      <label>CNO <input [(ngModel)]="modelo.cno" name="cno" /></label>
      <label>CNAE principal <input [(ngModel)]="modelo.cnaePrincipal" name="cnae" /></label>
      <fieldset>
        <legend>Endereço</legend>
        <label>Logradouro <input [(ngModel)]="modelo.endereco.logradouro" name="log" required /></label>
        <label>Número <input [(ngModel)]="modelo.endereco.numero" name="num" /></label>
        <label>Bairro <input [(ngModel)]="modelo.endereco.bairro" name="b" /></label>
        <label>Cidade <input [(ngModel)]="modelo.endereco.cidade" name="cid" required /></label>
        <label>UF <input [(ngModel)]="modelo.endereco.uf" name="uf" required maxlength="2" /></label>
        <label>CEP <input [(ngModel)]="modelo.endereco.cep" name="cep" /></label>
      </fieldset>
      <label>Certificado ID <input [(ngModel)]="modelo.certificadoId" name="cert" required /></label>
      <label>CPF do responsável <input [(ngModel)]="modelo.responsavelCpf" name="rcpf" required maxlength="11" /></label>
      <label>Nome do responsável <input [(ngModel)]="modelo.responsavelNome" name="rnome" required /></label>

      <button type="submit" [disabled]="!f.valid || enviando()">Salvar</button>
      @if (mensagem()) { <p>{{ mensagem() }}</p> }
    </form>
  `,
})
export class ConfiguracaoRepComponent implements OnInit {
  private readonly api = inject(Oficial671Service);
  enviando = signal(false);
  mensagem = signal<string | null>(null);

  modelo: ConfiguracaoRepPayload = {
    empresaId: '',
    tipo: 'RepC' as TipoRep,
    razaoSocial: '',
    cnpjCei: '',
    cno: null,
    inscricaoEstadual: null,
    cnaePrincipal: null,
    endereco: { logradouro: '', numero: null, complemento: null, bairro: null, cidade: '', uf: '', cep: null },
    certificadoId: '',
    responsavelCpf: '',
    responsavelNome: '',
  };

  ngOnInit(): void {}

  salvar(): void {
    this.enviando.set(true);
    this.mensagem.set(null);
    this.api.salvarConfiguracao(this.modelo).subscribe({
      next: (r) => { this.mensagem.set(`Salvo (${r.criada ? 'criada' : 'atualizada'}). ID=${r.configuracaoId}`); this.enviando.set(false); },
      error: (e) => { this.mensagem.set(`Erro: ${e.status} ${e.message}`); this.enviando.set(false); },
    });
  }
}
