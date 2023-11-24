INSERT INTO typearticle VALUES (2,'lifo'),(1,'fifo');

INSERT INTO article VALUES 
('sv','vary',1),
('svm001','vary mena',1),
('svs001','vary stock',1),
('svt001','vary tsipala',1),
('sp','pate',2),
('spp001','pate presto',2),
('sps001','pate sedap',2)
;

INSERT INTO magasin(nommagasin,lieu) VALUES ('magasin1','ambatondrazaka');




INSERT INTO entree(date,quantite,idarticle,idmagasin,prixunitaire) 
VALUES 
('2023-01-01',30,'svm001','magasin1',500),
('2023-01-10',50,'svm001','magasin1',600),
('2023-01-12',10,'svm001','magasin1',700),
('2023-01-02',10,'svt001','magasin1',200),
('2023-01-05',60,'svt001','magasin1',300),
('2023-01-10',60,'sps001','magasin1',800),
('2023-01-11',10,'sps001','magasin1',1000),
('2023-01-10',20,'spp001','magasin1',900),
('2023-01-11',10,'spp001','magasin1',1000)
;




