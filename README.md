**Tabela de Banco de // ..// =====================
// w_customer_entry.srw
// =====================

// Variáveis de instância
integer ii_mode  // 'NEW' ou 'EDIT'
integer ii_id
boolean of_saved

// Função de validação centralizada
public function boolean of_validate()
    string ls_name, ls_email, ls_phone

    ls_name  = dw_customer.getitemstring(1, "name")
    ls_email = dw_customer.getitemstring(1, "email")
    ls_phone = dw_customer.getitemstring(1, "phone")

    if trim(ls_name) = "" then
        MessageBox("Aviso", "Nome é obrigatório.")
        return FALSE
    end if

    if trim(ls_email) = "" or pos(ls_email, "@") = 0 then
        MessageBox("Aviso", "E-mail inválido.")
        return FALSE
    end if

    // Exemplo simples de validação de telefone
    if trim(ls_phone) = "" then
        MessageBox("Aviso", "Telefone é obrigatório.")
        return FALSE
    end if

    return TRUE
end function

// Método para configurar modo da janela
public subroutine of_setmode(string as_mode, integer ai_id)
    ii_mode = as_mode
    ii_id   = ai_id
end subroutine

// Evento OPEN
event open()
    if ii_mode = 'EDIT' then
        dw_customer.retrieve(ii_id)
    else
        dw_customer.insertrow(0)
    end if
    of_saved = FALSE
end event

// Botão Salvar
event clicked; // bt_Salvar
    if not this.of_validate() then return

    SQLCA.autocommit = false
    try
        if dw_customer.update() > 0 then
            if SQLCA.sqlcode = 0 then
                commit;
                MessageBox("Sucesso", "Cliente salvo com sucesso.")
                of_saved = TRUE
                close()
            else
                rollback;
                MessageBox("Erro", "Falha ao salvar.")
            end if
        else
            rollback;
            MessageBox("Erro", "Falha ao salvar dados.")
        end if
    catch (Throwable t)
        rollback;
        MessageBox("Erro", "Erro inesperado: " + t.getmessage())
    end try
    SQLCA.autocommit = true
end event

// Botão Cancelar
event clicked; // bt_Cancelar
    close()
end event

// =====================
// w_main.srw
// =====================

// Evento OPEN
event open()
    long ll_count
    ll_count = dw_customers.retrieve()
    if ll_count = 0 then
        MessageBox("Aviso", "Nenhum cliente encontrado.")
    end if
end event

// Botão Novo
event clicked; // bt_Novo
    w_customer_entry wce
    wce = CREATE w_customer_entry
    wce.of_setmode('NEW', 0)
    wce.openmodal()
    IF wce.of_saved = TRUE THEN
        dw_customers.retrieve()
    END IF
    DESTROY wce
end event

// Botão Editar
event clicked; // bt_Editar
    long    ll_row
    integer li_id

    ll_row = dw_customers.getrow()
    if ll_row < 1 then return

    li_id = dw_customers.getitemnumber(ll_row, 'customerid')

    w_customer_entry wce
    wce = CREATE w_customer_entry
    wce.of_setmode('EDIT', li_id)
    wce.openmodal()
    IF wce.of_saved = TRUE THEN
        dw_customers.retrieve()
    END IF
    DESTROY wce
end event

// Botão Excluir
event clicked; // bt_Excluir
    long ll_row
    ll_row = dw_customers.getrow()
    if ll_row < 1 then return

    if MessageBox("Confirmação", "Deseja excluir este cliente?", Question!, YesNo!) = Yes then
        SQLCA.autocommit = false
        try
            dw_customers.deleterow(ll_row)
            if dw_customers.update() > 0 and SQLCA.sqlcode = 0 then
                commit;
                MessageBox("Sucesso", "Cliente excluído com sucesso.")
            else
                rollback;
                MessageBox("Erro", "Falha ao excluir.")
            end if
        catch (Throwable t)
            rollback;
            MessageBox("Erro", "Erro inesperado: " + t.getmessage())
        end try
        SQLCA.autocommit = true
        dw_customers.retrieve()
    end if
end event

// Botão Consultar/Pesquisar
event clicked; // bt_Consultar
    string ls_filter
    ls_filter = sle_search.text
    if isnull(ls_filter) or trim(ls_filter) = '' then
        dw_customers.setfilter('')
    else
        dw_customers.setfilter("Upper(Name) like '%" + upper(ls_filter) + "%'")
    end if
    dw_customers.filter()
end event

// Botão Relatório
event clicked; // bt_Relatorio
    w_report wrpt
    wrpt = CREATE w_report
    wrpt.of_generate('')
    wrpt.open()
    DESTROY wrpt
end event

// =====================
// w_report.srw
// =====================

// Método para gerar relatório
public subroutine of_generate(string as_dummy)
    dw_customer_report.retrieve()
end subroutine

// Evento OPEN da janela de relatório
event open()
    dw_customer_report.printpreview()
end event.existing code...
event open()
    // Recupera os dados do relatório (caso ainda não tenha sido feito)
    this.of_generate("")
    // Exibe o relatório em modo de visualiz    event clicked()
        w_report wrpt
        wrpt = CREATE w_report
        wrpt.of_generate('')
        wrpt.open()
        DESTROY wrpt
    end event (SQL Server)**
`
