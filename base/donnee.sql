INSERT INTO typearticle VALUES (1,'lifo'),(2,'fifo');

INSERT INTO article VALUES 
('v','vary','1'),
('vm','vary mena','1'),
('p','pate','2'),
('pg','pate gasy','2')
;

INSERT INTO magasin(nommagasin,lieu) VALUES ('magasin1','ambatondrazaka');


INSERT INTO sortie(date,quantite,idarticle,idmagasin,etat) VALUES
('2023-11-05 12:00:00',10,'vm','magasin1',1)
;


INSERT INTO entree(date,quantite,idarticle,idmagasin) 
VALUES ('2023-11-02 12:30:00',10,'vm','magasin1')
;

INSERT INTO mouvement(date,idsortie,identree,quantite) VALUES
('2023-11-06 12:00:00','sortie1','entree1',15)
;



