namespace OpenMocap.Front.Services
{
    public class OperationConnectionStorage
    {
        private readonly Dictionary<Guid, List<string>> _operationConnection = new();
        private readonly Dictionary<string, Guid> _connectionOperation = new();

        public void Add(Guid operationId, string connectionId)
        {
            if (_operationConnection.TryGetValue(
                operationId,
                out var list))
            {
                list.Add(connectionId);
            }
            else
            {
                _operationConnection.Add(operationId, new() {connectionId });
                _connectionOperation.Add(connectionId, operationId);
            }
        }

        public void RemoveIfExists(string connectionId)
        {
            if(_connectionOperation.TryGetValue(connectionId, out var operation))
            {
                _connectionOperation.Remove(connectionId);
                _operationConnection.Remove(operation);
            }
        }

        public string[] GetConnections(Guid operationId)
        {
            if(_operationConnection.TryGetValue(operationId, out var list))
            {
                return list.ToArray();
            }

            return Array.Empty<string>();
        }

        public Guid[] Operations => _operationConnection.Keys.ToArray();
    }
}
