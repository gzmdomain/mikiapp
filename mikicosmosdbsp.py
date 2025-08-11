from azure.identity import ClientSecretCredential
from azure.cosmos import CosmosClient, exceptions
from random import randint

# Azure AD Service Principal 信息
tenant_id = "954ddad8-66d7-47a8-8f9f-1316152d9587"
client_id = "20d83853-d054-4ced-8a3d-b7da0d4f01e8"
client_secret = "Li6gg-4DeEt3AisKu-gt9YQ8_K-5Hzwlvh"

# Cosmos DB 信息
account_uri = "https://mikicosmosdbprd.mongo.cosmos.azure.cn:443/"  # 形如 https://xxxx.documents.azure.com:443/
database_name = "miki-mongodb-sample-database"
container_name = "unsharded-sample-collection"
sample_field_name = "sample_field"

# 获取 AAD 凭证
credential = ClientSecretCredential(
    tenant_id=tenant_id,
    client_id=client_id,
    client_secret=client_secret
)

# 创建 CosmosClient
client = CosmosClient(account_uri, credential=credential)
database = client.get_database_client(database_name)
container = database.get_container_client(container_name)

def insert_sample_document(container):
    doc = {sample_field_name: randint(50, 500)}
    result = container.create_item(body=doc)
    print("Inserted document:", result)
    return result['id']

def read_document(container, doc_id):
    item = container.read_item(item=doc_id, partition_key=doc_id)
    print("Read document:", item)

def update_document(container, doc_id):
    item = container.read_item(item=doc_id, partition_key=doc_id)
    item[sample_field_name] = "Updated!"
    updated = container.replace_item(item=doc_id, body=item)
    print("Updated document:", updated)

def delete_document(container, doc_id):
    container.delete_item(item=doc_id, partition_key=doc_id)
    print("Deleted document with id", doc_id)

def main():
    doc_id = insert_sample_document(container)
    read_document(container, doc_id)
    update_document(container, doc_id)
    delete_document(container, doc_id)

if __name__ == "__main__":
    main()