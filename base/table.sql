---Database  :  postgres  => stock

CREATE TABLE typearticle (
    idarticle SERIAL PRIMARY KEY,
    nomtype VARCHAR(100)
);


CREATE TABLE article (
    idarticle VARCHAR(100) PRIMARY KEY ,
    nom VARCHAR(100),
    type INTEGER,
    FOREIGN KEY(type) REFERENCES typearticle(idarticle)
);

CREATE TABLE magasin (
    idmagasin VARCHAR(100) DEFAULT 'magasin' || nextval('magasinsequence')::TEXT PRIMARY KEY,
    nommagasin VARCHAR(100),
    lieu VARCHAR(100)
);


CREATE TABLE sortie(
    idsortie VARCHAR(100) DEFAULT 'sortie' || nextval('sortiesequence')::TEXT PRIMARY KEY,
    date TIMESTAMP,
    quantite DOUBLE PRECISION,
    idarticle VARCHAR(100),
    idmagasin VARCHAR(100),
    etat INTEGER
);


CREATE TABLE entree (
    identree VARCHAR(100) DEFAULT 'entree' || nextval('entreesequence')::TEXT PRIMARY KEY,
    date TIMESTAMP,
    quantite DOUBLE PRECISION,
    idarticle VARCHAR(100),
    idmagasin VARCHAR(100),
    FOREIGN KEY(idarticle) REFERENCES article(idarticle),
    FOREIGN KEY(idmagasin) REFERENCES magasin(idmagasin)
);


CREATE TABLE mouvement (
    idmouvement VARCHAR(100) DEFAULT 'mouvement' || nextval('mouvementsequence')::TEXT PRIMARY KEY,
    date TIMESTAMP,
    idsortie VARCHAR(100),
    identree VARCHAR(100),
    quantite DOUBLE PRECISION,
    FOREIGN KEY(idsortie) REFERENCES sortie(idsortie),
    FOREIGN KEY(identree) REFERENCES entree(identree)
);


