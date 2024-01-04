using Subscription.Domain.Abstractions.Base;
using Subscription.Domain.Enum;

namespace Subscription.Domain.ValueObject
{
    public class Document : ValueObjectBase
    {
        public string DocumentNumber { get; private set; } = "";
        public DocumentType DocumentType { get; private set; }

        public static Document Create(string documentNumber)
        {
            Document document = new();
            document.ChangeDocumentNumber(documentNumber);
            return document;
        }

        private void ChangeDocumentNumber(string document)
        {
            if(document.Length == 11)
            {
                DocumentNumber = document;
                DocumentType = DocumentType.CPF;
            }else if(document.Length == 14)
            {
                DocumentNumber = document;
                DocumentType = DocumentType.CNPJ;
            }
            else
            {
                AddNotification("Documento invalido, 11 ou 14 caracteres");
                return;
            }
        }
    }
}
