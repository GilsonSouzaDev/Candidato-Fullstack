import re

with open('/opt/portfolio/nginx.conf', 'r') as f:
    config = f.read()

lines = config.split('\n')
in_atscandidato = False
for i, line in enumerate(lines):
    if 'server_name atscandidato.duckdns.org;' in line:
        in_atscandidato = True
    if in_atscandidato and 'location / {' in line:
        lines[i] = line.replace('location / {', 'location /candidato-api/ {')
    if in_atscandidato and 'proxy_pass http://141.148.17.246:8082/;' in line:
        lines[i] = line.replace('proxy_pass http://141.148.17.246:8082/;', 'proxy_pass http://candidato-backend:8080/api/;')
        break

with open('/tmp/nginx.conf', 'w') as f:
    f.write('\n'.join(lines))
