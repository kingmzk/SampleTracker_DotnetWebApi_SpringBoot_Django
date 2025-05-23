'''
import os
import pandas as pd
from django.core.management.base import BaseCommand
from modtest.models import OptyTracker, Accelerator, Competation, Microservice


class Command(BaseCommand):
    def handle(self, *args, **options):
        print("it is working!")
        excel_file = 'excel.xlsx'  # File name of your Excel file

        # Get the current working directory
        current_directory = os.path.dirname(os.path.abspath(__file__))

        # Construct the absolute file path
        file_path = os.path.join(current_directory, excel_file)

        # Read Excel sheet into a pandas DataFrame
        df = pd.read_excel(file_path)

        for _, row in df.iterrows():
            opty_tracker = OptyTracker()

            # Assign values from the row to the model fields
            opty_tracker.op_id = row['Opportunity ID'] if not pd.isnull(row['Opportunity ID']) else None
            opty_tracker.op_name = row['Opportunity Name'] if not pd.isnull(row['Opportunity Name']) else None
            opty_tracker.client_name = row['Client Name'] if not pd.isnull(row['Client Name']) else None

            # Handle many-to-many fields (accelerators, competations, microservices)
            accelerators_data = row['Accelerators']
            if not pd.isnull(accelerators_data):
                accelerators_list = [x.strip() for x in accelerators_data.split(',') if x.strip()]
                accelerators = []
                for accelerator_name in accelerators_list:
                    accelerator, _ = Accelerator.objects.get_or_create(acc_name=accelerator_name)
                    accelerators.append(accelerator)
                opty_tracker.accelerators.set(accelerators)

            competations_data = row['Competations']
            if not pd.isnull(competations_data):
                competations_list = [x.strip() for x in competations_data.split(',') if x.strip()]
                competations = []
                for competation_name in competations_list:
                    competation, _ = Competation.objects.get_or_create(name=competation_name)
                    competations.append(competation)
                opty_tracker.Competations.set(competations)

            microservices_data = row['Microservices']
            if not pd.isnull(microservices_data):
                microservices_list = [x.strip() for x in microservices_data.split(',') if x.strip()]
                microservices = []
                for microservice_name in microservices_list:
                    microservice, _ = Microservice.objects.get_or_create(name=microservice_name)
                    microservices.append(microservice)
                opty_tracker.Microservices.set(microservices)

            # Save the model instance
            opty_tracker.save()

        self.stdout.write(self.style.SUCCESS('Data imported successfully.'))
'''



'''

import os
import pandas as pd
from django.core.management.base import BaseCommand
from modtest.models import OptyTracker, Accelerator, Competation, Microservice


class Command(BaseCommand):
    def handle(self, *args, **options):
        print("it is working!")
        excel_file = 'excel.xlsx'  # File name of your Excel file

        # Get the current working directory
        current_directory = os.path.dirname(os.path.abspath(__file__))

        # Construct the absolute file path
        file_path = os.path.join(current_directory, excel_file)

        # Read Excel sheet into a pandas DataFrame
        df = pd.read_excel(file_path)

        for _, row in df.iterrows():
            # Create a new OptyTracker instance
            opty_tracker = OptyTracker()

            # Assign values from the row to the model fields
            opty_tracker.op_name = row['Opportunity Name'] if 'Opportunity Name' in row else None
            opty_tracker.client_name = row['Client Name'] if 'Client Name' in row else None

            # Save the model instance to generate the ID
            opty_tracker.save()

            # Handle many-to-many fields (accelerators, competitions, microservices)
            accelerators_data = row['Accelerators'] if 'Accelerators' in row else None
            if accelerators_data:
                accelerators_list = [x.strip() for x in accelerators_data.split(',') if x.strip()]
                accelerators = []
                for accelerator_name in accelerators_list:
                    accelerator, _ = Accelerator.objects.get_or_create(acc_name=accelerator_name)
                    accelerators.append(accelerator)
                opty_tracker.accelerators.set(accelerators)

            competations_data = row['Competations'] if 'Competations' in row else None
            if competations_data:
                competations_list = [x.strip() for x in competations_data.split(',') if x.strip()]
                competations = []
                for competation_name in competations_list:
                    competation, _ = Competation.objects.get_or_create(name=competation_name)
                    competations.append(competation)
                opty_tracker.Competations.set(competations)

            microservices_data = row['Microservices'] if 'Microservices' in row else None
            if microservices_data:
                microservices_list = [x.strip() for x in microservices_data.split(',') if x.strip()]
                microservices = []
                for microservice_name in microservices_list:
                    microservice, _ = Microservice.objects.get_or_create(name=microservice_name)
                    microservices.append(microservice)
                opty_tracker.Microservices.set(microservices)

        self.stdout.write(self.style.SUCCESS('Data imported successfully.'))
'''





# commands/excel_to_database.py

import os
import pandas as pd
from django.core.management.base import BaseCommand
from modtest.models import OptyTracker, Accelerator, Competation, Microservice


class Command(BaseCommand):
    def handle(self, *args, **options):
        print("it is working!")
        excel_file = 'excel.xlsx'  # File name of your Excel file

        # Get the current working directory
        current_directory = os.path.dirname(os.path.abspath(__file__))

        # Construct the absolute file path
        file_path = os.path.join(current_directory, excel_file)

        # Read Excel sheet into a pandas DataFrame
        df = pd.read_excel(file_path)

        for _, row in df.iterrows():
            # Create a new OptyTracker instance
            opty_tracker = OptyTracker()

            # Assign values from the row to the model fields
            opty_tracker.op_id = row['Opportunity ID'] if 'Opportunity ID' in row else None
            opty_tracker.op_name = row['Opportunity Name'] if 'Opportunity Name' in row else None
            opty_tracker.client_name = row['Client Name'] if 'Client Name' in row else None

            # Save the model instance
            opty_tracker.save()

            # Handle many-to-many fields (accelerators, competations, microservices)
            accelerators_data = row['Accelerators'] if 'Accelerators' in row else None
            if accelerators_data:
                accelerators_list = [x.strip() for x in accelerators_data.split(',') if x.strip()]
                accelerators = []
                for accelerator_name in accelerators_list:
                    accelerator, _ = Accelerator.objects.get_or_create(acc_name=accelerator_name)
                    accelerators.append(accelerator)
                opty_tracker.accelerators.set(accelerators)

            competations_data = row['Competations'] if 'Competations' in row else None
            if competations_data:
                competations_list = [x.strip() for x in competations_data.split(',') if x.strip()]
                competations = []
                for competation_name in competations_list:
                    competation, _ = Competation.objects.get_or_create(name=competation_name)
                    competations.append(competation)
                opty_tracker.competations.set(competations)

            microservices_data = row['Microservices'] if 'Microservices' in row else None
            if microservices_data:
                microservices_list = [x.strip() for x in microservices_data.split(',') if x.strip()]
                microservices = []
                for microservice_name in microservices_list:
                    microservice, _ = Microservice.objects.get_or_create(name=microservice_name)
                    microservices.append(microservice)
                opty_tracker.microservices.set(microservices)

        self.stdout.write(self.style.SUCCESS('Data imported successfully.'))
