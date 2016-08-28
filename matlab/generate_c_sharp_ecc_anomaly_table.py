import csv

ecc_str = '0.0002981'
description_string = 'Look-up table mapping mean anomaly to eccentric anomaly to the Earth-ISS system'
ecc_str_name_version = ecc_str.replace('.', '_')

csv_filename = 'mean_to_ecc_anom_' + ecc_str + '.csv'

c_sharp_output_filename = 'mean_to_ecc_lookup_table.cs'
with open(c_sharp_output_filename, 'w') as codefile:
	codefile.write('// ' + description_string + '\n')
	codefile.write('public static List<double> eccentricAnomalies' + ecc_str_name_version +' = new List<double>(\n')

	with open(csv_filename, 'rb') as csvfile:
		csv_reader = csv.reader(csvfile)
		for row in csv_reader:
			mean_anomaly = row[0]
			ecc_anomaly = row[1]
			codefile.write('\t' + ecc_anomaly + ', // mean anomaly: ' + mean_anomaly + '\n')

	codefile.write(');\n\n')
